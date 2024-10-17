interface IEntityState<T> {
  entity: T | null;
  loaded: boolean;
}

interface IEntitiesState<T> {
  entities: T[] | null;
  loaded: boolean;
}

export type EntityState<T, C extends boolean = false> = C extends true ? IEntityState<T> & { checked: boolean } : IEntityState<T>;
export type EntitiesState<T, C extends boolean = false> = C extends true ? IEntitiesState<T> & { checked: boolean } : IEntitiesState<T>;
