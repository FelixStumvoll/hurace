import { combineReducers } from 'redux';
import { connectRouter, RouterState } from 'connected-react-router';
import { History } from 'history';
import {
    seasonExpanderReducer,
    SeasonExpanderReducerState
} from './reducers/season-expander-reducer/seasonExpanderReducer';

export const rootReducer = (history: History<any>) =>
    combineReducers({
        seasonExpander: seasonExpanderReducer,
        router: connectRouter(history)
    });

export type StoreState = {
    router: RouterState<any>;
    seasonExpander: SeasonExpanderReducerState;
};
