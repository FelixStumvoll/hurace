import {
    SeasonExpanderAction,
    SeasonExpanderActionType
} from './seasonExpanderActionTypes';
import { Reducer } from 'redux';

export interface SeasonExpanderReducerState {
    seasonExpanded?: number;
}

const initialState = {
    seasonExpanded: undefined
};

export const seasonExpanderReducer: Reducer<
    SeasonExpanderReducerState,
    SeasonExpanderAction
> = (state = initialState, action: SeasonExpanderAction) => {
    switch (action.type) {
        case SeasonExpanderActionType.Expand:
            return {
                seasonExpanded: action.id
            };
        case SeasonExpanderActionType.Collapse:
            return {
                seasonExpanded: undefined
            };
        default:
            return state;
    }
};
