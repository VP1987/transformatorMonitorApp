<template>
  <div class="page-wrapper">
    <HeaderComponent @add-card="showAddModal = true" />

    <main class="page">
      <CardComponent
        v-for="card in cardsStore.cards"
        :key="card.id"
        :card="card"
      />
    </main>

    <AddCardModal v-if="showAddModal" @close="showAddModal = false" />

    <CardSettingsModal />
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from "vue";

import HeaderComponent from "@/presentation/components/HeaderComponent.vue";
import CardComponent from "@/presentation/components/card/CardComponent.vue";

import AddCardModal from "@/presentation/components/modals/AddCardModal.vue";
import CardSettingsModal from "@/presentation/components/modals/CardSettingsModal.vue";
import { useCardsStore } from "@/application/stores/card.store";
import { useTransformersStore } from "@/application/stores/transformers.store";

const cardsStore = useCardsStore();
const transformersStore = useTransformersStore();

const showAddModal = ref(false);

onMounted(async () => {
  await cardsStore.load();
  await transformersStore.load();
});
</script>
