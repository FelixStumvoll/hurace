import { combineReducers } from 'redux';
import { connectRouter, RouterState } from 'connected-react-router';
import { History } from 'history';

import {
    AuthReducerState,
    authReducer
} from './reducers/auth-reducer/authReducer';

export const rootReducer = (history: History<any>) =>
    combineReducers({
        auth: authReducer,
        router: connectRouter(history)
    });

export type StoreState = {
    router: RouterState<any>;
    auth: AuthReducerState;
};
