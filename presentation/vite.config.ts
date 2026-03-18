import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import fs from 'node:fs'
import path from 'node:path'

// Function to find app-config.json in parent directories
const findConfig = () => {
  let currentDir = fileURLToPath(new URL('.', import.meta.url));
  while (currentDir !== path.parse(currentDir).root) {
    const configPath = path.join(currentDir, 'app-config.json');
    if (fs.existsSync(configPath)) {
      return JSON.parse(fs.readFileSync(configPath, 'utf-8'));
    }
    currentDir = path.dirname(currentDir);
  }
  return null;
};

const config = findConfig();
const frontendPort = config?.Frontend?.Port ?? 5173;

export default defineConfig({
  plugins: [vue()],
  server: {
    port: frontendPort,
    strictPort: true,
    host: true // Needed for Docker and network access
  },
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  test: {
    globals: true,
    environment: 'jsdom',
  }
})
