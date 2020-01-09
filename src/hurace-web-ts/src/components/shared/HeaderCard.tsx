import React from 'react';
import styled, { CSSProperties } from 'styled-components';
import { Card } from '../../theme/CustomComponents';

const Panel = styled(Card)`
    overflow: hidden;
    padding: 0px;
    display: flex;
    flex-direction: column;
`;

const CardHeader = styled.div`
    padding: 5px 5px 5px 10px;
    font-weight: bold;
    background-color: ${props => props.theme.black};
    color: white;
`;

const CardContent = styled.div`
    padding: ${props => props.theme.gap};
    height: calc(100% - (${props => props.theme.gap} + ${props => props.theme.gap}));
    width: calc(100% - (${props => props.theme.gap} + ${props => props.theme.gap}));
    background-color: rgb(235, 236, 240);
    /* overflow: auto; */
`;

export const HeaderCard: React.FC<{
    headerText: string;
    contentStyles?: CSSProperties;
}> = ({ headerText, children, contentStyles }) => {
    return (
        <Panel>
            <CardHeader>{headerText}</CardHeader>
            <CardContent style={contentStyles}>{children}</CardContent>
        </Panel>
    );
};
