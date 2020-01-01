import React from 'react';
import styled from 'styled-components';
import { StartList } from '../../../../interfaces/StartList';

const StartListViewItemPanel = styled.div`
    border-bottom: 1px solid rgba(128, 128, 128, 0.5);
    display: flex;
    :last-child {
        border-bottom: none;
    }
`;

const StartNumber = styled.div`
    font-weight: bold;
`;

const RightAlign = styled.div`
    margin: 0 5px 0 auto;
`;

export const StartListViewItem: React.FC<{ startList: StartList }> = ({
    startList
}) => {
    return (
        <StartListViewItemPanel>
            <StartNumber>{startList.startNumber}</StartNumber>
            <RightAlign>
                {startList.skier.firstName} {startList.skier.lastName} -{' '}
                {startList.skier.country.countryCode}
            </RightAlign>
        </StartListViewItemPanel>
    );
};
