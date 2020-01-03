import React, { useEffect } from 'react';
import createAuth0Client from '@auth0/auth0-spa-js';
import { useDispatch } from 'react-redux';
import { replace } from 'connected-react-router';
import {
    login,
    setAuth0Client
} from './store/reducers/auth-reducer/authActions';

export const Auth0Provider: React.FC<Auth0ClientOptions> = ({
    children,
    ...initOptions
}) => {
    const dispatch = useDispatch();
    useEffect(() => {
        const initAuth0 = async () => {
            const auth0Client = await createAuth0Client(initOptions);
            dispatch(setAuth0Client(auth0Client));
            if (window.location.search.includes('code=')) {
                const { appState } = await auth0Client.handleRedirectCallback();
                dispatch(
                    replace(
                        appState && appState.targetUrl
                            ? appState.targetUrl
                            : '/'
                    )
                );
            }

            if (await auth0Client.isAuthenticated()) {
                const user = await auth0Client.getUser();
                dispatch(login(user));
            }
        };
        initAuth0();
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

    return <>{children}</>;
};
