import type { TransformerDto } from "@/domain/DTOs/TransformerDto";

const STORAGE_KEY = "transformers.runtime";
const TARGET_ASSET_ID = 15;
const MAX_READINGS = 10;

export class TransformerRuntimeEngine {
  private timerId: number | null = null;

  start(intervalMs = 30_000) {
    if (this.timerId !== null) return;

    this.timerId = window.setInterval(() => {
      const raw = localStorage.getItem(STORAGE_KEY);
      if (!raw) return;

      const transformers: TransformerDto[] = JSON.parse(raw);

      const index = transformers.findIndex(
        (t) => t.assetId === TARGET_ASSET_ID
      );
      if (index === -1) return;

      const transformer = transformers[index];
      if (!transformer) return;

      const newReading = {
        timestamp: new Date().toISOString(),
        voltage: String(Math.floor(Math.random() * (35000 - 5000 + 1)) + 5000),
      };

      transformers[index] = {
        assetId: transformer.assetId,
        name: transformer.name,
        region: transformer.region,
        health: transformer.health,
        lastTenVoltageReadings: [
          newReading,
          ...transformer.lastTenVoltageReadings.slice(0, MAX_READINGS - 1),
        ],
      };

      localStorage.setItem(STORAGE_KEY, JSON.stringify(transformers));
    }, intervalMs);
  }

  stop() {
    if (this.timerId !== null) {
      clearInterval(this.timerId);
      this.timerId = null;
    }
  }
}
