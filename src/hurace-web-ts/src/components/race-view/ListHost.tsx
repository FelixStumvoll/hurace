import React from 'react';
import styled from 'styled-components';
import { Card } from '../../theme/StyledComponents';

const ListPanel = styled(Card)`
    overflow: hidden;
    display: grid;
    grid-template-rows: auto 1fr;
`;

const ListHeader = styled.div`
    font-weight: bold;
`;

const ListItems = styled.div`
    overflow: auto;
    height: 100%;
`;

export const ListHost: React.FC<{ headerText: string }> = ({
    headerText,
    children
}) => {
    return (
        <ListPanel>
            <ListHeader>{headerText}</ListHeader>
            <ListItems>{children}</ListItems>
        </ListPanel>
    );
};
