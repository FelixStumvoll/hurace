import React from 'react';
import styled from 'styled-components';
import { NavLink } from 'react-router-dom';

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

export const Navbar: React.FC = () => {
    return (
        <Nav>
            <NavbarNavLink
                to="/seasons"
                gridarea="season"
                activeStyle={{ textDecoration: 'underline' }}
            >
                Saisonüberblick
            </NavbarNavLink>
            <NavbarNavLink
                to="/skiers"
                gridarea="skiers"
                activeStyle={{ textDecoration: 'underline' }}
            >
                Rennfahrer
            </NavbarNavLink>
        </Nav>
    );
};
