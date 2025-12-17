export type HealthViewFilter = "All" | "Good" | "Poor" | "Critical";

export interface ViewSettings {
  health: HealthViewFilter;
  limit: number;
}
