import React from 'react';
import styled from 'styled-components';
import { Card } from '../../theme/StyledComponents';

const ListPanel = styled(Card)`
    overflow: hidden;
    padding: 0px;
    display: grid;
    grid-template-rows: auto 1fr;
`;

const ListHeader = styled.div`
    padding: 5px 5px 5px 10px;
    font-weight: bold;
    background-color: ${props => props.theme.black};
    color: white;
`;

const ListItems = styled.div`
    padding: 0 10px 10px 10px;
    height: calc(100% - 10px);
    width: calc(100% - 20px);
    overflow: auto;
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
