import React from 'react';
import styled from 'styled-components';

const Nav = styled.nav`
    position: fixed;
    top: 0px;
    left: 0px;
    width: 100%;
    height: ${props => props.theme.navHeight};
    background-color: #2a2a2a;
`;

export const Navbar: React.FC = () => {
    return <Nav />;
};
