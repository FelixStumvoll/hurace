import React, { useState, useEffect } from 'react';
import { StartList } from '../../../../interfaces/StartList';
import { StartListViewItem } from './StartListViewItem';
import { ListHost } from '../../ListHost';
import { setStateAsync } from '../../../../common/stateSetter';
import { getStartListForRace } from '../../../../api';

export const StartListView: React.FC<{ raceId: number }> = ({ raceId }) => {
    const [startList, setStartList] = useState<StartList[] | undefined>(
        undefined
    );

    useEffect(() => {
        if (startList !== undefined) return;
        setStateAsync(setStartList, getStartListForRace(raceId));
    }, [startList, raceId]);

    return (
        <ListHost headerText="Startliste:">
            {startList?.map(sl => (
                <StartListViewItem startList={sl} />
            ))}
        </ListHost>
    );
};
