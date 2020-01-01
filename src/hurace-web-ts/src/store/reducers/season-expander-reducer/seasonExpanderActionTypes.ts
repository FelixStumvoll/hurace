export enum SeasonExpanderActionType {
    Expand,
    Collapse
}

export type SeasonExpanderAction =
    | {
          type: SeasonExpanderActionType.Expand;
          id: number;
      }
    | { type: SeasonExpanderActionType.Collapse };
