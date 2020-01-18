import React, { useEffect, createContext, useState, useCallback } from 'react';
import createAuth0Client from '@auth0/auth0-spa-js';
import Auth0Client from '@auth0/auth0-spa-js/dist/typings/Auth0Client';
import { useHistory } from 'react-router-dom';
import { User } from '../models/User';

interface AuthContextProps {
    user?: User;
    isAuthenticated: boolean;
    token?: string;
    auth0Client?: Auth0Client;
    login: () => void;
    logout: () => void;
}

export const AuthContext = createContext<AuthContextProps>(
    {} as AuthContextProps
);

export const Auth0Provider: React.FC<Auth0ClientOptions> = ({
    children,
    ...initOptions
}) => {
    const [user, setUser] = useState<User>();
    const [auth0Client, setAuth0Client] = useState<Auth0Client>();
    const [token, setToken] = useState<string>();

    const history = useHistory();

    const checkAuthenticatedUser = useCallback(async (client: Auth0Client) => {
        if (await client.isAuthenticated()) {
            setToken(await client.getTokenSilently());
            setUser(await client.getUser());
        }
    }, []);

    const login = useCallback(async () => {
        await auth0Client?.loginWithPopup();
        auth0Client && (await checkAuthenticatedUser(auth0Client));
    }, [auth0Client, checkAuthenticatedUser]);

    const logout = useCallback(() => {
        auth0Client?.logout();
        setUser(undefined);
        setToken(undefined);
    }, [auth0Client]);

    useEffect(() => {
        const initAuth = async () => {
            const client = await createAuth0Client(initOptions);
            setAuth0Client(client);

            if (window.location.search.includes('code=')) {
                const { appState } = await client.handleRedirectCallback();

                history.replace(
                    appState && appState.targetUrl ? appState.targetUrl : '/'
                );
            }
            // if (await client.isAuthenticated()) {
            //     setToken(await client.getTokenSilently());
            //     setUser(await client.getUser());
            // }
            checkAuthenticatedUser(client);
        };

        initAuth();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return (
        <AuthContext.Provider
            value={{
                auth0Client,
                isAuthenticated: user !== undefined,
                token,
                user,
                login,
                logout
            }}
        >
            {children}
        </AuthContext.Provider>
    );
};
