import { createApp } from "vue";
import { createPinia } from "pinia";
import App from "./App.vue";
import { setupCrossTabSync } from "@/shared/persistence/crossTabSync";
import "@/assets/theme.css";
import { TransformerRuntimeEngine } from "@/mock/TransformerRuntimeEngine";

const engine = new TransformerRuntimeEngine();
engine.start();

const app = createApp(App);
app.use(createPinia());
setupCrossTabSync();
app.mount("#app");
