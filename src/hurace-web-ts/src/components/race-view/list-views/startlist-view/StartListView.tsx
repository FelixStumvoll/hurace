import React, { useState, useEffect } from 'react';
import { StartList } from '../../../../interfaces/StartList';
import { StartListViewItem } from './StartListViewItem';
import { setStateAsync } from '../../../../common/stateSetter';
import { getStartListForRace } from '../../../../common/api';
import styled from 'styled-components';
import { HeaderCard } from '../../../shared/HeaderCard';

const StartListTable = styled.table`
    width: 100%;
`;

export const StartListView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [startList, setStartList] = useState<StartList[] | undefined>(
        undefined
    );

    useEffect(() => {
        if (startList !== undefined) return;
        setStateAsync(setStartList, getStartListForRace(raceId));
    }, [startList, raceId]);

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
