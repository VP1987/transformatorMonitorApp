import { defineStore } from "pinia";
import { ref, watchEffect } from "vue";

export const useUIStore = defineStore("ui", () => {
  const isQuickViewOpen = ref(false);
  const selectedTransformerId = ref<number | null>(null);

  const getInitialTheme = () => {
    const saved = localStorage.getItem("theme_preference");
    return saved ? saved === "dark" : true;
  };

  const isDarkMode = ref(getInitialTheme());

  function toggleTheme() {
    isDarkMode.value = !isDarkMode.value;
    const theme = isDarkMode.value ? "dark" : "light";

    document.documentElement.setAttribute("data-theme", theme);
    localStorage.setItem("theme_preference", theme);
  }

  watchEffect(() => {
    const theme = isDarkMode.value ? "dark" : "light";
    document.documentElement.setAttribute("data-theme", theme);
  });

  function openQuickView(id: number) {
    selectedTransformerId.value = id;
    isQuickViewOpen.value = true;
  }

  function closeQuickView() {
    isQuickViewOpen.value = false;
    selectedTransformerId.value = null;
  }

  return {
    isQuickViewOpen,
    selectedTransformerId,
    isDarkMode,
    toggleTheme,
    openQuickView,
    closeQuickView,
  };
});
