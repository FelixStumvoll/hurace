import React from 'react';
import { RouteProps, Route, Redirect } from 'react-router-dom';
import { AuthContext } from '../Auth0Provider';

export const AuthRoute: React.FC<RouteProps & {
    redirectUri: string;
}> = ({ redirectUri, children, ...rest }) => {
    return (
        <AuthContext.Consumer>
            {({ isAuthenticated }) =>
                isAuthenticated ? (
                    <Route {...rest}>{children}</Route>
                ) : (
                    <Redirect to={redirectUri} />
                )
            }
        </AuthContext.Consumer>
    );
};
