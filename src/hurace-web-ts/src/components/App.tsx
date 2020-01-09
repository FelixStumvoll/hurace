import React from 'react';
import styled, { ThemeProvider } from 'styled-components';
import { HuraceTheme } from '../theme/HuraceTheme';
import { Navbar } from './Navbar';
import { Router } from './Router';
import { Auth0Provider } from './Auth0Provider';
import { env } from '../environment/environment';
import { BrowserRouter } from 'react-router-dom';

const PageContent = styled.div`
    display: grid;
    grid-template-areas: '.' 'main';
    height: 100%;
    grid-template-rows: ${props => props.theme.navHeight} 1fr;
`;

const MainContent = styled.main`
    grid-area: main;
    height: calc(
        100% - (${props => props.theme.gap} + ${props => props.theme.gap})
    );
    width: calc(
        100% - (${props => props.theme.gap} + ${props => props.theme.gap})
    );
    padding: ${props => props.theme.gap};
`;

export const App: React.FC = () => {
    return (
        <BrowserRouter>
            <Auth0Provider
                client_id={env.auth0ClientId}
                domain={env.auth0Domain}
            >
                <ThemeProvider theme={HuraceTheme}>
                    <PageContent>
                        <Navbar />
                        <MainContent>
                            <Router />
                        </MainContent>
                    </PageContent>
                </ThemeProvider>
            </Auth0Provider>
        </BrowserRouter>
    );
};
