<template>
  <div class="app-container">
    <MainPage v-if="uiStore.currentPage === 'dashboard'" />
    <MaintenancePage v-if="uiStore.currentPage === 'maintenance'" />
    
    <AlertNotifier />
    <TransformerQuickView />

    <!-- Global Theme Toggle (Bottom Right) -->
    <button class="theme-fab" @click="uiStore.toggleTheme" title="Toggle Theme">
      <svg v-if="uiStore.isDarkMode" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><path d="M21 12.79A9 9 0 1111.21 3 7 7 0 0021 12.79z" /></svg>
      <svg v-else width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2"><circle cx="12" cy="12" r="5"/><line x1="12" y1="1" x2="12" y2="3"/><line x1="12" y1="21" x2="12" y2="23"/><line x1="4.22" y1="4.22" x2="5.64" y2="5.64"/><line x1="18.36" y1="18.36" x2="19.78" y2="19.78"/><line x1="1" y1="12" x2="3" y2="12"/><line x1="21" y1="12" x2="23" y2="12"/><line x1="4.22" y1="19.78" x2="5.64" y2="18.36"/><line x1="18.36" y1="5.64" x2="19.78" y2="4.22"/></svg>
    </button>
  </div>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted } from "vue";
import { useTransformersStore } from "@/application/stores/transformers.store";
import { useUIStore } from "@/application/stores/ui.store";
import MainPage from "@/presentation/views/MainPage.vue";
import MaintenancePage from "@/presentation/views/MaintenancePage.vue";
import AlertNotifier from "@/presentation/components/shared/AlertNotifier.vue";
import TransformerQuickView from "@/presentation/components/modals/TransformerQuickView.vue";

const store = useTransformersStore();
const uiStore = useUIStore();

onMounted(() => {
  store.load();
});

onUnmounted(() => {
  store.dispose();
});
</script>

<style scoped>
.app-container {
  min-height: 100vh;
  position: relative;
}

.theme-fab {
  position: fixed;
  bottom: 24px;
  right: 24px;
  width: 48px;
  height: 48px;
  border-radius: 50%;
  background: var(--card-bg);
  border: 1px solid var(--border);
  color: var(--text);
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  z-index: 2000;
  transition: all 0.2s;
}

.theme-fab:hover {
  background: var(--btn-bg);
  transform: translateY(-2px);
}

.theme-fab:active {
  transform: scale(0.9);
}
</style>
