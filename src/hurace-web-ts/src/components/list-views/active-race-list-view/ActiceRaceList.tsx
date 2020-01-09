import React from 'react';
import { useStateAsync } from '../../../hooks/useStateAsync';
import { getActiveRaces } from '../../../common/api';
import { ListViewWrapper } from '../../shared/ListViewWrapper';
import { RaceListViewItem } from '../../detail-views/season-detail-view/discipline-view/RaceListViewItem';
import { DefaultLink } from '../../../theme/CustomComponents';

export const ActiceRaceList: React.FC = () => {
    const [activeRaces] = useStateAsync(getActiveRaces);

    return (
        <ListViewWrapper>
            {activeRaces?.map(ar => (
                <DefaultLink key={ar.id} to={`/activeRace/${ar.id}`}>
                    <RaceListViewItem race={ar} />
                </DefaultLink>
            ))}
        </ListViewWrapper>
    );
};
