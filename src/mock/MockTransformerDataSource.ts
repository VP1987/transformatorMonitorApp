import type { TransformerDataSource } from "@/application/datasources/TransformerDataSource";
import type { TransformerDto } from "@/domain/DTOs/TransformerDto";

const STORAGE_KEY = "transformers.runtime";
const INTERVALS_KEY = "transformers.intervals";

export class MockTransformerDataSource implements TransformerDataSource {
  private timer: number | null = null;
  private cb: ((data: TransformerDto[]) => void) | null = null;

  public updateConfig(ids: number[], intervalSeconds: number): void {
    const ms = intervalSeconds * 1000;
    const rawIntervals = localStorage.getItem(INTERVALS_KEY);
    const intervals: Record<number, number> = rawIntervals
      ? JSON.parse(rawIntervals)
      : {};

    ids.forEach((id) => {
      intervals[id] = ms;
    });

    localStorage.setItem(INTERVALS_KEY, JSON.stringify(intervals));
  }

  public subscribe(cb: (data: TransformerDto[]) => void): void {
    this.cb = cb;
    const raw = localStorage.getItem(STORAGE_KEY);
    if (raw) cb(JSON.parse(raw));
    this.timer = window.setInterval(() => this.emit(), 1000);
  }

  public unsubscribe(): void {
    if (this.timer !== null) {
      clearInterval(this.timer);
      this.timer = null;
    }
    this.cb = null;
  }

  private emit(): void {
    if (!this.cb) return;

    const rawData = localStorage.getItem(STORAGE_KEY);
    const rawIntervals = localStorage.getItem(INTERVALS_KEY);

    if (!rawData) return;

    const list: TransformerDto[] = JSON.parse(rawData);
    const intervals: Record<number, number> = rawIntervals
      ? JSON.parse(rawIntervals)
      : {};

    const now = new Date().getTime();
    let hasChanges = false;

    const newList = list.map((t) => {
      const targetInterval = intervals[t.assetId] || 300000;
      const readings = t.lastTenVoltageReadings || [];
      const lastReading = readings[0];

      const lastTime = lastReading
        ? new Date(lastReading.timestamp).getTime()
        : 0;

      if (now - lastTime >= targetInterval) {
        hasChanges = true;

        const currentVoltage = lastReading
          ? Number(lastReading.voltage)
          : 22000;

        const variation = (Math.random() - 0.5) * 400;
        const newVoltage = Math.round(currentVoltage + variation);

        readings.unshift({
          timestamp: new Date().toISOString(),
          voltage: newVoltage.toString(),
        });

        if (readings.length > 10) {
          readings.pop();
        }

        return {
          ...t,
          lastTenVoltageReadings: [...readings],
        };
      }
      return t;
    });

    if (hasChanges) {
      localStorage.setItem(STORAGE_KEY, JSON.stringify(newList));
      this.cb(newList);
    } else {
      this.cb(list);
    }
  }
}
