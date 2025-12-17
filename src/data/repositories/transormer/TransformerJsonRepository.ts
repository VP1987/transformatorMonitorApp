import type { ITransformerRepository } from "./ITransformerRepository";
import type { Transformer } from "@/domain/entities/transformer/Transformer";
import type { TransformerDto } from "@/domain/DTOs/TransformerDto";
import { mapTransformerDtoToDomain } from "@/domain/mappers/transformerMapper";

const STORAGE_KEY = "transformers.runtime";

export class TransformerJsonRepository implements ITransformerRepository {
  async getAll(): Promise<Transformer[]> {
    const cached = localStorage.getItem(STORAGE_KEY);

    if (cached) {
      const data: TransformerDto[] = JSON.parse(cached);
      return data.map(mapTransformerDtoToDomain);
    }

    const response = await fetch("/data/sampledata.json");
    if (!response.ok) {
      throw new Error("Failed to load transformer data");
    }

    const data: TransformerDto[] = await response.json();

    localStorage.setItem(STORAGE_KEY, JSON.stringify(data));

    return data.map(mapTransformerDtoToDomain);
  }
}
