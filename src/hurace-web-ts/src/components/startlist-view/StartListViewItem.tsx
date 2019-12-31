import React from 'react';
import styled from 'styled-components';
import { StartList } from '../../interfaces/StartList';

const StartListViewItemPanel = styled.div`
    border-bottom: 1px solid rgba(128, 128, 128, 0.5);
    display: flex;
`;

const StartNumber = styled.div`
    font-weight: bold;
`;

export const StartListViewItem: React.FC<{ startList: StartList }> = ({
    startList
}) => {
    return (
        <StartListViewItemPanel>
            <StartNumber>{startList.startNumber}</StartNumber>
            <div>{startList.skier.firstName}</div>
            <div>{startList.skier.lastName}</div>
        </StartListViewItemPanel>
    );
};
