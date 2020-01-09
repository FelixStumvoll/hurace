import React from 'react';
import { useStateAsync } from '../../hooks/useStateAsync';
import { getActiveRaces } from '../../common/api';
import { ListViewWrapper } from '../shared/ListViewWrapper';
import { RaceListViewItem } from '../shared/race/RaceListViewItem';
import { DefaultLink, FlexWrap } from '../../theme/CustomComponents';

export const ActiceRaceList: React.FC = () => {
    const [activeRaces] = useStateAsync(getActiveRaces);

    return (
        <ListViewWrapper>
            <FlexWrap>
                {activeRaces?.map(ar => (
                    <DefaultLink key={ar.id} to={`/activeRace/${ar.id}`}>
                        <RaceListViewItem race={ar} />
                    </DefaultLink>
                ))}
            </FlexWrap>
        </ListViewWrapper>
    );
};
