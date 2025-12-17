<script setup lang="ts">
import { onMounted, onUnmounted } from "vue";
import { useTransformersStore } from "@/application/stores/transformers.store";
import MainPage from "@/presentation/views/MainPage.vue";
import AlertNotifier from "@/presentation/components/shared/AlertNotifier.vue";
import TransformerQuickView from "@/presentation/components/modals/TransformerQuickView.vue";

const store = useTransformersStore();

onMounted(() => {
  store.load();
});

onUnmounted(() => {
  store.dispose();
});
</script>

<template>
  <MainPage />
  <AlertNotifier />
  <TransformerQuickView />
</template>
