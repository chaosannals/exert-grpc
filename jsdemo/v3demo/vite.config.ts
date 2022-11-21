import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
// mkcert will open a root cert register messagebox
// basic ssl is only make cert, but it is offical.
// import mkcert from'vite-plugin-mkcert'
import basicSsl from '@vitejs/plugin-basic-ssl'

// https://vitejs.dev/config/
export default defineConfig({
  build: {
    commonjsOptions: {
      // include: /node_modules|grpc/,
      // transformMixedEsModules: true,
      // defaultIsModuleExports: true,
      // defaultIsModuleExports: 'auto',
    }
  },
  server: {
    https: true,
    proxy: {
      '^/JsDemo': {
        target: 'http://127.0.0.1:50051',
        changeOrigin: true,
      }
    },
  },
  plugins: [
    vue(),
    basicSsl(),
    // mkcert(),
  ]
})
