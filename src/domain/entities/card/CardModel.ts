export interface CardSettings {
  health: "All" | "Good" | "Fair" | "Poor" | "Critical";
  limit: number;
  sortBy: "id" | "name" | "region" | "health";
  sortDir: "asc" | "desc";
  showChart: boolean;
  search?: string;
  selectedTransformerIds?: number[];
  refreshInterval: number;
}

export interface CardModel {
  id: string;
  title: string;
  type: "transformer";
  settings: CardSettings;
}
