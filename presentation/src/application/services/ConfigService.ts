export interface AppConfig {
  Backend: {
    Port: number;
    Url: string;
  };
  Frontend: {
    Port: number;
    Url: string;
  };
}

let config: AppConfig | null = null;

export const loadConfig = async (): Promise<AppConfig> => {
  if (config) {
    return config;
  }
  const response = await fetch("/app-config.json");
  config = await response.json();
  return config!;
};

export const getConfig = (): AppConfig => {
  if (!config) {
    throw new Error("Configuration not loaded. Call loadConfig() first.");
  }
  return config;
};

export const getApiUrl = (): string => `${getConfig().Backend.Url}/api`;
export const getHubUrl = (): string => `${getConfig().Backend.Url}/hubs/transformers`;
