import { defineStore } from "pinia";
import type { Transformer } from "@/domain/entities/transformer/Transformer";
import { MockTransformerDataSource } from "@/mock/MockTransformerDataSource";
import { mapTransformerDtoToDomain } from "@/domain/mappers/transformerMapper";
import { alertService } from "@/application/services/AlertService";
import type { TransformerDto } from "@/domain/DTOs/TransformerDto";

const STORAGE_KEY = "transformers.runtime";

export const useTransformersStore = defineStore("transformers", {
  state: () => ({
    transformers: [] as Transformer[],
    isLoaded: false,
    dataSource: null as MockTransformerDataSource | null,
  }),

  actions: {
    async load() {
      if (this.isLoaded) return;

      if (!localStorage.getItem(STORAGE_KEY)) {
        const res = await fetch("/data/sampledata.json");
        if (!res.ok) throw new Error("Failed to load sample data");
        const data = await res.json();
        localStorage.setItem(STORAGE_KEY, JSON.stringify(data));
      }

      const ds = new MockTransformerDataSource();
      this.dataSource = ds;

      ds.subscribe((dtos: TransformerDto[]) => {
        const domainModels = dtos.map(mapTransformerDtoToDomain);
        this.transformers = domainModels;

        alertService.processTransformers(domainModels);

        this.isLoaded = true;
      });
    },

    configureMonitoring(ids: number[], intervalSeconds: number) {
      if (this.dataSource) {
        this.dataSource.updateConfig(ids, intervalSeconds);
      }
    },

    dispose() {
      this.dataSource?.unsubscribe();
      this.dataSource = null;
      this.isLoaded = false;
    },
  },
});
