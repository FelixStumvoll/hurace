import React from 'react';
import styled from 'styled-components';
import { NavLink } from 'react-router-dom';
// import { useAuth0 } from '../Auth0Provider';
import { useSelector, useDispatch } from 'react-redux';
import { StoreState } from '../store/rootReducer';
import {
    loginRedirect,
    logoutAuth0
} from '../store/reducers/auth-reducer/authActions';

const Nav = styled.nav`
    position: fixed;
    top: 0px;
    left: 0px;
    width: 100%;
    height: ${props => props.theme.navHeight};
    background-color: #2a2a2a;
    display: grid;
    grid-template-areas: 'season skiers . login';
    grid-template-columns: auto auto 1fr auto;
`;

interface NavbarNavLinkProps {
    gridarea: string;
}

const NavbarNavLink = styled(NavLink)<NavbarNavLinkProps>`
    grid-area: ${props => props.gridarea};
    text-decoration: none;
    color: white;
    margin: auto 0px auto 10px;
`;

const LoginButton = styled.button`
    grid-area: login;
    margin: auto 10px auto 0;
    height: fit-content;
`;

export const Navbar: React.FC = () => {
    // const { loginWithRedirect, logout } = useAuth0();
    const { isAuthenticated } = useSelector((state: StoreState) => state.auth);

    const dispatch = useDispatch();
    return (
        <Nav>
            <NavbarNavLink
                to="/season"
                gridarea="season"
                activeStyle={{ textDecoration: 'underline' }}
            >
                Saisonüberblick
            </NavbarNavLink>
            <NavbarNavLink
                to="/skier"
                gridarea="skiers"
                activeStyle={{ textDecoration: 'underline' }}
            >
                Rennfahrer
            </NavbarNavLink>
            {isAuthenticated ? (
                <LoginButton onClick={() => dispatch(logoutAuth0())}>
                    Logout
                </LoginButton>
            ) : (
                <LoginButton onClick={() => dispatch(loginRedirect())}>
                    Login
                </LoginButton>
            )}
        </Nav>
    );
};
