import { AuthAction, AuthActionType } from './authActionTypes';
import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';
import { StoreState } from '../../rootReducer';
import { Dispatch } from 'react';

export const loginRedirect = () => async (
    dispatch: Dispatch<any>,
    getState: () => StoreState
) => {
    let state = getState();
    let { auth0Client } = state.auth;
    if (!auth0Client) return;

    await auth0Client!.loginWithRedirect({
        redirect_uri: 'http://localhost:3000'
    });
};

export const login = (user: any, token: string): AuthAction => ({
    token,
    user,
    type: AuthActionType.Login
});

export const logoutAuth0 = () => async (
    dispatch: Dispatch<any>,
    getState: () => StoreState
) => {
    let { auth0Client } = getState().auth;
    if (!auth0Client) return;

    await auth0Client.logout();
    dispatch(logout);
};

const logout = (): AuthAction => ({ type: AuthActionType.Logout });

export const setAuth0Client = (auth0Client: Auth0Client): AuthAction => ({
    type: AuthActionType.SetAuth0Client,
    auth0Client
});
