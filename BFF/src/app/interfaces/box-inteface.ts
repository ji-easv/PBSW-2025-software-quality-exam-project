import { Dimensions, DimensionsDto } from "./dimension-interface";


export interface PaginatedBoxList {
  boxes: Box[];
  pageCount: number;
}

export interface Box {
  id: string; // Guids are represented as strings in Typescript
  weight: number; // float numbers are represented as number type in TypeScript
  colour?: string; // string? is represented as Optional String in TypeScript
  material?: string; // string? is represented as Optional String in TypeScript
  dimensions?: Dimensions; // Assuming Dimensions is another interface we defined in TypeScript
  createdAt: Date; // DateTime in C# = Date in TypeScript
  stock: number; // int in C# = number in TypeScript
  price: number; // float in C# = number in TypeScript
}

export interface BoxCreateDto {
  weight: number;
  colour?: string | null;
  material?: string | null;
  dimensionsDto?: DimensionsDto | null; // Assuming Dimensions is another interface we defined in TypeScript
  stock: number;
  price: number;
}

export interface BoxUpdateDto {
  weight: number;
  colour?: string | null;
  material?: string | null;
  dimensionsDto?: DimensionsDto | null; // Assuming Dimensions is another interface defined
  stock: number; // int in C# = number in TypeScript
  price: number; // float in C# = number in TypeScript
}
