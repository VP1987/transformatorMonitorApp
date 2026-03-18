import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import { setupCrossTabSync } from "@/shared/persistence/crossTabSync";
import "@/assets/theme.css";
import { loadConfig } from "@/application/services/ConfigService";

const init = async () => {
  await loadConfig();

  const app = createApp(App);
  app.use(createPinia());
  setupCrossTabSync();
  app.mount("#app");
};

init();
