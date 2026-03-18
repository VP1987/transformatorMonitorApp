import type { ITransformerRepository } from "./ITransformerRepository";
import type { Transformer } from "@/domain/entities/transformer/Transformer";
import { mapTransformerDtoToDomain } from "@/domain/mappers/transformerMapper";
import { getApiUrl } from "@/application/services/ConfigService";

export class TransformerApiRepository implements ITransformerRepository {
  async getAll(): Promise<Transformer[]> {
    const response = await fetch(`${getApiUrl()}/transformers`);
    if (!response.ok) {
      throw new Error("failedToLoadTransformerData");
    }

    const data = await response.json();
    return data.map(mapTransformerDtoToDomain);
  }
}
