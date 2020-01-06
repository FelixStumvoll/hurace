import React from 'react';
import { StartListViewItem } from './StartListViewItem';
import { getStartListForRace } from '../../../../common/api';
import styled from 'styled-components';
import { HeaderCard } from '../../../shared/HeaderCard';
import { useStateAsync } from '../../../../hooks/asyncState';

const StartListTable = styled.table`
    width: 100%;
`;

export const StartListView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [startList] = useStateAsync(getStartListForRace, raceId);

    return (
        <HeaderCard headerText="Startliste:">
            <StartListTable>
                <thead>
                    <tr>
                        <th align="left">Startnr.</th>
                        <th align="left">Land</th>
                        <th align="left">Name</th>
                    </tr>
                </thead>
                <tbody>
                    {startList?.map(sl => (
                        <StartListViewItem
                            key={sl.startNumber}
                            startList={sl}
                        />
                    ))}
                </tbody>
            </StartListTable>
        </HeaderCard>
    );
};
