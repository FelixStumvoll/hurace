import {
    SeasonExpanderAction,
    SeasonExpanderActionType
} from './seasonExpanderActionTypes';

export const expandSeason = (id: number): SeasonExpanderAction => ({
    id,
    type: SeasonExpanderActionType.Expand
});

export const collapseSeason = (): SeasonExpanderAction => ({
    type: SeasonExpanderActionType.Collapse
});
