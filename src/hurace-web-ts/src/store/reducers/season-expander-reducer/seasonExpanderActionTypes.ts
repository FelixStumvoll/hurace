export enum SeasonExpanderActionType {
    Expand = 'EXPAND',
    Collapse = 'COLLAPSE'
}

export type SeasonExpanderAction =
    | {
          type: SeasonExpanderActionType.Expand;
          id: number;
      }
    | { type: SeasonExpanderActionType.Collapse };
