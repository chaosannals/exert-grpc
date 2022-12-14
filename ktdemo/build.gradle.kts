buildscript {
    repositories {
        // maven { setUrl("https://maven.aliyun.com/repository/public") }
        // maven { setUrl("https://maven.aliyun.com/repository/google") }
        // maven { setUrl("https://maven.aliyun.com/repository/grails-core") }
        // maven { setUrl("https://maven.aliyun.com/repository/gradle-plugin") }
        // mavenLocal()
        maven { setUrl("https://repo.huaweicloud.com/repository/maven") }
        mavenCentral()
    }
}

plugins {
    kotlin("jvm") version "1.7.21" apply false
    id("com.google.protobuf") version "0.8.19" apply false
}

ext["grpcVersion"] = "1.51.0"
ext["grpcKotlinVersion"] = "1.3.0"
ext["protobufVersion"] = "3.21.11"
ext["coroutinesVersion"] = "1.6.4"

allprojects {
    repositories {
        // maven { setUrl("https://maven.aliyun.com/repository/public") }
        // maven { setUrl("https://maven.aliyun.com/repository/google") }
        // maven { setUrl("https://maven.aliyun.com/repository/grails-core") }
        // maven { setUrl("https://maven.aliyun.com/repository/gradle-plugin") }
        // mavenLocal()
        maven { setUrl("https://repo.huaweicloud.com/repository/maven") }
        mavenCentral()
    }
}
