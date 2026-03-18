<template>
  <header class="app-header">
    <div class="left">
      <div class="logo">⚡</div>
      <div class="titles">
        <h1>Transformer Monitor</h1>
        <p>Real-time Asset Health & Monitoring</p>
      </div>
    </div>

    <!-- Navigacija skroz desno -->
    <nav class="right-nav">
      <button 
        :class="['nav-item', { active: uiStore.currentPage === 'dashboard' }]"
        @click="uiStore.navigateTo('dashboard')"
      >
        Dashboard
      </button>
      <button 
        :class="['nav-item', { active: uiStore.currentPage === 'maintenance' }]"
        @click="uiStore.navigateTo('maintenance')"
      >
        Maintenance
      </button>
    </nav>

    <!-- Theme toggle je izbacen odavde -->
  </header>
</template>

<script setup lang="ts">
import { useUIStore } from "@/application/stores/ui.store";
const uiStore = useUIStore();
</script>

<style scoped>
.app-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 24px;
  background: var(--bg);
  border-bottom: 1px solid var(--border);
  height: 60px;
  box-sizing: border-box;
}

.left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.logo {
  background: var(--btn-bg);
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
}

.titles h1 { font-size: 15px; font-weight: 700; margin: 0; }
.titles p { font-size: 11px; color: var(--muted); margin: 0; }

.right-nav {
  display: flex;
  gap: 4px;
}

.nav-item {
  background: transparent;
  border: none;
  color: var(--muted);
  font-size: 13px;
  font-weight: 600;
  padding: 8px 16px;
  border-radius: 8px;
  cursor: pointer;
  transition: all 0.2s;
}

.nav-item:hover {
  color: var(--text);
  background: var(--btn-bg);
}

.nav-item.active {
  color: var(--accent);
  background: var(--btn-bg);
}
</style>
