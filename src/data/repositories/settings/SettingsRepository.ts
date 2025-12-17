import type { ViewSettings } from "@/domain/settings/ViewSettings";

export class SettingsRepository {
  async getViewSettings(): Promise<ViewSettings> {
    const res = await fetch("/data/settings.json");
    if (!res.ok) {
      throw new Error("Failed to load settings");
    }
    return res.json();
  }
}
