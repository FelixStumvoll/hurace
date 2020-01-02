import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';

export enum AuthActionType {
    Login = 'LOGIN',
    Logout = 'LOGOUT',
    SetAuth0Client = 'SETAUTHCLIENT'
}

export type AuthAction =
    | { type: AuthActionType.Login; user: any }
    | { type: AuthActionType.SetAuth0Client; auth0Client: Auth0Client }
    | { type: AuthActionType.Logout };
