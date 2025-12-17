import { useTransformersStore } from "@/application/stores/transformers.store";

export function setupCrossTabSync() {
  window.addEventListener("storage", (e) => {
    if (e.key !== "transformer-app-state" || !e.newValue) return;
    const store = useTransformersStore();
    Object.assign(store.$state, JSON.parse(e.newValue));
  });
}
