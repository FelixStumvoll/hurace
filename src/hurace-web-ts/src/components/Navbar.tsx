import React from 'react';
import styled from 'styled-components';
import { NavLink } from 'react-router-dom';
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
    border-bottom: 6px solid transparent;
    display: flex;
    margin-left: 10px;
`;

const NavbarContent = styled.div`
    margin: auto 0 auto 0;
`;

const LoginButton = styled.button`
    grid-area: login;
    margin-left: 10px;
    height: fit-content;
    background-color: ${props => props.theme.blue};
    color: white;
    border: none;
    height: 100%;
    padding: 0 15px 0 15px;
    cursor: pointer;
`;

export const Navbar: React.FC = () => {
    const { isAuthenticated } = useSelector((state: StoreState) => state.auth);

    const dispatch = useDispatch();
    let activeStyle = '6px solid #0078F2';
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
                to="/skier"
                gridarea="skiers"
                activeStyle={{ borderBottom: activeStyle }}
            >
                <NavbarContent>Rennfahrer</NavbarContent>
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
