import React, { useContext } from 'react';
import styled, { ThemeContext } from 'styled-components';
import { NavLink } from 'react-router-dom';
import { AuthContext } from './Auth0Provider';

const Nav = styled.nav`
    position: fixed;
    top: 0px;
    left: 0px;
    width: calc(100% - ${props => props.theme.gap});
    padding-left: ${props => props.theme.gap};
    height: ${props => props.theme.navHeight};
    background-color: ${props => props.theme.black};
    display: grid;
    gap: ${props => props.theme.gap};
    grid-template-areas: 'season active skiers . name login';
    grid-template-columns: auto auto auto 1fr auto;
`;

interface NavbarNavLinkProps {
    gridarea: string;
}

const NavbarNavLink = styled(NavLink)<NavbarNavLinkProps>`
    grid-area: ${props => props.gridarea};
    text-decoration: none;
    color: white;
    border-bottom: 6px solid transparent;
    display: flex;
`;

const NavbarContent = styled.div`
    margin: auto 0 auto 0;
`;

const LoginButton = styled.button`
    grid-area: login;
    height: fit-content;
    background-color: ${props => props.theme.blue};
    color: white;
    border: none;
    height: 100%;
    padding: 0 15px 0 15px;
    cursor: pointer;
    font-family: ${props => props.theme.fontFamily};
`;

const Username = styled.div`
    grid-area: name;
    color: white;
    height: fit-content;
    margin: auto;
`;

export const Navbar: React.FC = () => {
    const theme = useContext(ThemeContext);

    let activeStyle = `6px solid ${theme.blue}`;

    return (
        <Nav>
            <NavbarNavLink
                to="/season"
                gridarea="season"
                activeStyle={{ borderBottom: activeStyle }}
            >
                <NavbarContent>Saisonüberblick</NavbarContent>
            </NavbarNavLink>
            <NavbarNavLink
                to="/activeRace"
                gridarea="active"
                activeStyle={{ borderBottom: activeStyle }}
            >
                <NavbarContent>Laufende Rennen</NavbarContent>
            </NavbarNavLink>
            <NavbarNavLink
                to="/skier"
                gridarea="skiers"
                activeStyle={{ borderBottom: activeStyle }}
            >
                <NavbarContent>Rennfahrer</NavbarContent>
            </NavbarNavLink>
            <AuthContext.Consumer>
                {({ login, logout, isAuthenticated, user }) =>
                    isAuthenticated ? (
                        <>
                            <Username>{user?.name}</Username>
                            <LoginButton onClick={logout}>Logout</LoginButton>
                        </>
                    ) : (
                        <LoginButton onClick={login}>Login</LoginButton>
                    )
                }
            </AuthContext.Consumer>
        </Nav>
    );
};
