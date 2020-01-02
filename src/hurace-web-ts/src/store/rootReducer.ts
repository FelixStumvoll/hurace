import { combineReducers } from 'redux';
import { connectRouter, RouterState } from 'connected-react-router';
import { History } from 'history';
import {
    seasonExpanderReducer,
    SeasonExpanderReducerState
} from './reducers/season-expander-reducer/seasonExpanderReducer';
import {
    AuthReducerState,
    authReducer
} from './reducers/auth-reducer/authReducer';

export const rootReducer = (history: History<any>) =>
    combineReducers({
        seasonExpander: seasonExpanderReducer,
        auth: authReducer,
        router: connectRouter(history)
    });

export type StoreState = {
    router: RouterState<any>;
    auth: AuthReducerState;
    seasonExpander: SeasonExpanderReducerState;
};
