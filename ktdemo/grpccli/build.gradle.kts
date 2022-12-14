import org.jetbrains.kotlin.gradle.tasks.KotlinCompile
import com.google.protobuf.gradle.generateProtoTasks
import com.google.protobuf.gradle.id
import com.google.protobuf.gradle.plugins
import com.google.protobuf.gradle.protobuf
import com.google.protobuf.gradle.protoc

plugins {
    kotlin("jvm")
    id("com.google.protobuf")
    application
}

group = "exert.grpc"
version = "1.0-SNAPSHOT"

kotlin {
    jvmToolchain {
        languageVersion.set(JavaLanguageVersion.of(11))
    }
}

dependencies {
    api("org.jetbrains.kotlinx:kotlinx-coroutines-core:${rootProject.ext["coroutinesVersion"]}")

    api("io.grpc:grpc-stub:${rootProject.ext["grpcVersion"]}")
    api("io.grpc:grpc-protobuf:${rootProject.ext["grpcVersion"]}")
    api("com.google.protobuf:protobuf-java-util:${rootProject.ext["protobufVersion"]}")
    api("com.google.protobuf:protobuf-kotlin:${rootProject.ext["protobufVersion"]}")
    api("io.grpc:grpc-kotlin-stub:${rootProject.ext["grpcKotlinVersion"]}")

    runtimeOnly("io.grpc:grpc-netty:${rootProject.ext["grpcVersion"]}")

   testImplementation(kotlin("test"))
}

tasks.test {
    useJUnitPlatform()
}

tasks.withType<KotlinCompile> {
    kotlinOptions.jvmTarget = "11"
}

application {
    mainClass.set("exert.grpc.democli.AppKt")
}

protobuf {
    protoc {
        artifact = "com.google.protobuf:protoc:${rootProject.ext["protobufVersion"]}"
    }
    plugins {
        id("grpc") {
            artifact = "io.grpc:protoc-gen-grpc-java:${rootProject.ext["grpcVersion"]}"
        }
        id("grpckt") {
            artifact = "io.grpc:protoc-gen-grpc-kotlin:${rootProject.ext["grpcKotlinVersion"]}:jdk8@jar"
        }
    }
    generateProtoTasks {
        all().forEach {
            it.plugins {
                id("grpc")
                id("grpckt")
            }
            it.builtins {
                id("kotlin")
            }
        }
    }
}