import React from 'react';
import { getActiveRaces } from '../../common/api';
import { RaceListViewItem } from '../shared/race/RaceListViewItem';
import {
    DefaultLink,
    FlexWrap,
    NoListEntryText
} from '../../theme/CustomComponents';
import { LoadingWrapper } from '../shared/LoadingWrapper';
import { useAsync } from 'react-async-hook';
import { compareRace } from '../../common/compareFunctions';

export const ActiceRaceList: React.FC = () => {
    const { loading, error, result: activeRaces } = useAsync(
        getActiveRaces,
        []
    );

    return (
        <LoadingWrapper loading={loading} error={error}>
            <FlexWrap>
                {activeRaces &&
                    (activeRaces.length === 0 ? (
                        <NoListEntryText>
                            Derzeit keine aktiven Rennen
                        </NoListEntryText>
                    ) : (
                        activeRaces.sort(compareRace).map(ar => (
                            <DefaultLink
                                key={ar.id}
                                to={`/activeRace/${ar.id}`}
                            >
                                <RaceListViewItem race={ar} />
                            </DefaultLink>
                        ))
                    ))}
            </FlexWrap>
        </LoadingWrapper>
    );
};
