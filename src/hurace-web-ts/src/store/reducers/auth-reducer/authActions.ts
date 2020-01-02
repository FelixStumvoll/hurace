import { AuthAction, AuthActionType } from './authActionTypes';
import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';
import { StoreState } from '../../rootReducer';
import { Dispatch } from 'react';

export const loginRedirect = () => async (
    dispatch: any,
    getState: () => StoreState
) => {
    let state = getState();
    console.log('state', state);
    let { auth0Client } = state.auth;
    if (!auth0Client) return;

    await auth0Client!.loginWithRedirect();
    const user = await auth0Client!.getUser();
    
    dispatch(login(user));
};

export const login = (user: any): AuthAction => ({
    user: user,
    type: AuthActionType.Login
});

export const logoutAuth0 = () => async (
    dispatch: Dispatch<any>,
    state: StoreState
) => {
    let { auth0Client } = state.auth;
    if (!auth0Client) return;

    await auth0Client.logout();
    dispatch(logout);
};

const logout = (): AuthAction => ({ type: AuthActionType.Logout });

export const setAuth0Client = (auth0Client: Auth0Client): AuthAction => ({
    type: AuthActionType.SetAuth0Client,
    auth0Client
});
