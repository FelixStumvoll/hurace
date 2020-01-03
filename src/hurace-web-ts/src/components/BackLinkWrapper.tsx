import React from 'react';
import styled from 'styled-components';
import { Link } from 'react-router-dom';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faAngleLeft } from '@fortawesome/free-solid-svg-icons';

const BackLinkPanel = styled.div`
    display: grid;
    grid-template-rows: auto 1fr;
    row-gap: 10px;
    height: 100%;
`;

const BackLink = styled(Link)`
    text-decoration: none;
    color: black;
    font-size: 20px;
`;

const BackIcon = styled(FontAwesomeIcon)`
    margin-right: 5px;
`;

export const BackLinkWrapper: React.FC<{ backText: string; url: string }> = ({
    backText,
    url,
    children
}) => {
    return (
        <BackLinkPanel>
            <BackLink to={url}>
                <BackIcon icon={faAngleLeft}></BackIcon>
                <span>{backText}</span>
            </BackLink>
            {children}
        </BackLinkPanel>
    );
};
