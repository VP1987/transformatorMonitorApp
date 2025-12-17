<template>
  <header class="app-header">
    <div class="left">
      <div class="logo">⚡</div>
      <div class="titles">
        <h1>Transformer Monitor</h1>
        <p>Real-time Asset Health & Monitoring</p>
      </div>
    </div>

    <button class="burger" @click="isMobileMenuOpen = !isMobileMenuOpen">
      <span></span>
      <span></span>
      <span></span>
    </button>

    <div class="right" :class="{ 'mobile-open': isMobileMenuOpen }">
      <button class="action-btn" @click="handleAddCard">
        <span class="plus">+</span>
        <span class="label">Add monitor</span>
      </button>

      <button class="theme-toggle" @click="uiStore.toggleTheme">
        <svg
          v-if="uiStore.isDarkMode"
          width="18"
          height="18"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
          <path d="M21 12.79A9 9 0 1111.21 3 7 7 0 0021 12.79z" />
        </svg>
        <svg
          v-else
          width="18"
          height="18"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
        >
          <circle cx="12" cy="12" r="5" />
          <line x1="12" y1="1" x2="12" y2="3" />
          <line x1="12" y1="21" x2="12" y2="23" />
          <line x1="4.22" y1="4.22" x2="5.64" y2="5.64" />
          <line x1="18.36" y1="18.36" x2="19.78" y2="19.78" />
          <line x1="1" y1="12" x2="3" y2="12" />
          <line x1="21" y1="12" x2="23" y2="12" />
          <line x1="4.22" y1="19.78" x2="5.64" y2="18.36" />
          <line x1="18.36" y1="5.64" x2="19.78" y2="4.22" />
        </svg>
      </button>
    </div>
  </header>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useUIStore } from "@/application/stores/ui.store";

const emit = defineEmits<{
  (e: "add-card"): void;
}>();

const uiStore = useUIStore();
const isMobileMenuOpen = ref(false);

function handleAddCard() {
  emit("add-card");
  isMobileMenuOpen.value = false;
}
</script>

<style scoped>
.app-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 12px 24px;
  background: var(--bg);
  border-bottom: 1px solid var(--border);
  position: relative;
  z-index: 100;
}

.left {
  display: flex;
  align-items: center;
  gap: 12px;
}

.logo {
  background: var(--btn-bg);
  color: var(--text);
  width: 32px;
  height: 32px;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: 8px;
  font-size: 14px;
}

.titles h1 {
  font-size: 15px;
  font-weight: 700;
  margin: 0;
  letter-spacing: -0.01em;
}

.titles p {
  font-size: 11px;
  color: var(--muted);
  margin: 0;
}

.right {
  display: flex;
  align-items: center;
  gap: 8px;
}

.action-btn,
.theme-toggle {
  height: 36px;
  padding: 0 12px;
  border-radius: 8px;
  background: var(--btn-bg);
  border: 1px solid var(--border);
  color: var(--text);
  cursor: pointer;
  display: flex;
  align-items: center;
  gap: 8px;
  font-size: 13px;
  font-weight: 500;
  transition: all 0.2s ease;
}

.action-btn:hover,
.theme-toggle:hover {
  background: var(--btn-hover-bg);
}

.plus {
  font-size: 18px;
  font-weight: 400;
}

.label {
  display: inline-block;
}

.burger {
  display: none;
  flex-direction: column;
  gap: 4px;
  background: none;
  border: none;
  cursor: pointer;
  padding: 4px;
}

.burger span {
  width: 20px;
  height: 2px;
  background: var(--text);
  border-radius: 2px;
}

@media (max-width: 768px) {
  .app-header {
    padding: 12px 16px;
  }

  .burger {
    display: flex;
  }

  .label {
    display: block;
  }

  .right {
    position: absolute;
    top: 100%;
    left: 0;
    right: 0;
    background: var(--bg);
    flex-direction: column;
    padding: 16px;
    gap: 12px;
    border-bottom: 1px solid var(--border);
    display: none;
    box-shadow: 0 10px 15px -3px rgba(0, 0, 0, 0.1);
  }

  .right.mobile-open {
    display: flex;
  }

  .action-btn,
  .theme-toggle {
    width: 100%;
    justify-content: center;
    height: 44px;
  }

  .titles p {
    display: none;
  }
}
</style>
