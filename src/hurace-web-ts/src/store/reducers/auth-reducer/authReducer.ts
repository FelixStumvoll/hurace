import { AuthAction, AuthActionType } from './authActionTypes';
import { Reducer } from 'redux';
import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';

export interface AuthReducerState {
    isAuthenticated: boolean;
    user?: any;
    token?: string;
    auth0Client?: Auth0Client;
}

const initialState: AuthReducerState = {
    isAuthenticated: false,
    user: undefined,
    auth0Client: undefined
};

export const authReducer: Reducer<AuthReducerState, AuthAction> = (
    state = initialState,
    action: AuthAction
) => {
    switch (action.type) {
        case AuthActionType.Login:
            return {
                ...state,
                isAuthenticated: true,
                user: action.user,
                token: action.token
            };
        case AuthActionType.Logout:
            return {
                ...state,
                isAuthenticated: false,
                user: undefined,
                token: undefined
            };
        case AuthActionType.SetAuth0Client:
            return { ...state, auth0Client: action.auth0Client };
        default:
            return state;
    }
};
